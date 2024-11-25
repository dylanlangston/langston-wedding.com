import * as React from 'react';
import { HeartParticleAnimator } from './HeartParticleAnimator';
import BackgroundWorker from './Background.worker?worker'
import { IPCMessage, IPCMessageType } from '../../lib/IPCMessage';
import { sanitizeEvent } from '../../lib/sanitizeEvent';
import { debounce } from '../../lib/debounce';

interface BackgroundProps {
    workerUnsupported?: boolean
}

function createCanvas(): HTMLCanvasElement {
    const canvas = document.createElement('canvas')
    canvas.style.display = 'block';
    canvas.style.position = 'fixed';
    canvas.style.top = '0';
    canvas.style.left = '0';
    canvas.style.zIndex = '-1000';
    canvas.style.width = '100vw';
    canvas.style.height = '100vh';
    canvas.style.pointerEvents = 'none';
    canvas.addEventListener('contextmenu', (ev) => ev.preventDefault());
    return canvas;
}

const workerSupported = typeof HTMLCanvasElement.prototype.transferControlToOffscreen === 'function' && typeof Worker !== 'undefined'

const Background: React.FC<BackgroundProps> = ({ workerUnsupported = !workerSupported }) => {
    const divRef = React.useRef<HTMLDivElement | null>(null);

    React.useEffect(() => {
        if (!divRef.current) return;
        
        const canvas = createCanvas();
        divRef.current.appendChild(canvas);

        if (workerUnsupported) {
            canvas.width = window.innerWidth;
            canvas.height = window.innerHeight;

            const ctx = canvas.getContext('2d');
            if (!ctx) return;

            const animator = new HeartParticleAnimator(ctx);

            const handleResize = debounce(() => {
                canvas.width = window.innerWidth;
                canvas.height = window.innerHeight;
                animator.drawNow();
            }, 10);

            const handlePointer = (ev: any) => {
                const { x, y } = ev;
                animator.addHeart(x, y);
            }
        
            window.addEventListener('resize', handleResize);
            window.addEventListener('pointermove', handlePointer);
            window.addEventListener('pointerdown', handlePointer);

            return () => {
                animator.stopAnimation();
                canvas.remove();
                window.removeEventListener('resize', handleResize);
                window.removeEventListener('pointermove', handlePointer);
                window.removeEventListener('pointerdown', handlePointer);
            };
        }
        else {
            let worker: Worker | undefined;
            const listeners: ((() => void) | null)[] = []
            const startWorkerTimeout = setTimeout(() => {
                const HandleEvent = (
                    target: EventTarget,
                    add: boolean,
                    eventHandler: {
                        id: number
                        target: string
                        type: string
                    }
                ) => {
                    if (add) {
                        const postMessage = (_type: string, m: IPCMessage) => {
                            worker?.postMessage(m)
                        }
                        const listener = (e: Event) =>
                            postMessage(
                                eventHandler.type,
                                IPCMessage.EventHandlerCallback({
                                    id: eventHandler.id,
                                    target: eventHandler.target,
                                    type: eventHandler.type,
                                    event: sanitizeEvent(e)
                                })
                            )
                        listeners[eventHandler.id] = () => {
                            target.removeEventListener(eventHandler.type, listener)
                        }
                        target.addEventListener(eventHandler.type, listener)
                    } else {
                        listeners[eventHandler.id]?.();
                        listeners[eventHandler.id] = null;
                    }
                }
    
                const GetTarget: (eventHandler: { target: string }) => EventTarget = (eventHandler) => {
                    switch (eventHandler.target) {
                        case 'Window':
                            return window
                        case 'Document':
                            return document;
                        default:
                            throw 'Not Implemented'
                    }
                }
    
                const GetMediaQuery = (mediaQuery: string) => window.matchMedia(mediaQuery)
    
                worker = new BackgroundWorker();
                canvas.width = window.innerWidth;
                canvas.height = window.innerHeight;
                const offscreenCanvas = canvas.transferControlToOffscreen();
    
                const onMessage = (ev: MessageEvent<IPCMessage>) => {
                    switch (ev.data.type) {
                        case IPCMessageType.AddEventHandler:
                            HandleEvent(GetTarget(ev.data.message as any), true, ev.data.message as any)
                            break
                        case IPCMessageType.RemoveEventHandler:
                            HandleEvent(GetTarget(ev.data.message as any), false, ev.data.message as any)
                            break
                        case IPCMessageType.AddMediaQueryHandler:
                            HandleEvent(GetMediaQuery((ev.data.message as any).target), true, ev.data.message as any)
                            break
                        case IPCMessageType.RemoveMediaQueryHandler:
                            HandleEvent(GetMediaQuery((ev.data.message as any).target), false, ev.data.message as any)
                            break
                    }
                }
                const onError = (er: ErrorEvent) => console.error(er)
    
                worker.addEventListener('message', onMessage)
                worker.addEventListener('error', onError)
    
                worker.postMessage(IPCMessage.Initialize(offscreenCanvas), [offscreenCanvas])    
            }, 100)

            return () => {
                clearTimeout(startWorkerTimeout)
                worker?.terminate();
                canvas.remove();
                listeners.forEach(cleanup => cleanup?.())
            }
        }
    }, [divRef.current]);

    return <div ref={divRef}></div>
};

export default Background;