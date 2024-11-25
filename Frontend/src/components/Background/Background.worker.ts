import { IPCMessage, IPCMessageDataType, IPCMessageType } from "../../lib/IPCMessage";
import { IPCProxy } from "../../lib/IPCProxy";
import { WorkerDOM } from "../../lib/WorkerDom";
import { HeartParticleAnimator } from "./HeartParticleAnimator";
import { debounce } from "../../lib/debounce";
import { sleep } from "../../lib/sleep";

let eventHandlers: { [type: number]: (message: IPCMessageDataType) => void; } = {};
// On Initialized
eventHandlers[IPCMessageType.Initialize] = (message: IPCMessageDataType) => {
    const eventDetails: {
        canvas: OffscreenCanvas
    } = <any>message;

    const canvas = new WorkerDOM.OffscreenCanvasExtended(eventDetails.canvas);
    self.document = new WorkerDOM.Document(canvas);

    postMessage(IPCMessage.Initialized());

    const ctx = canvas.getContext('2d');
    if (!ctx) return;

    const animator = new HeartParticleAnimator(ctx);

    // Handle resize with a smooth animation
    let runningInstance: number;
    const handleResize = debounce(async (ev: any) => {
        const { width, height } = ev.target;
        const canvas = self.document.getCanvas();
        const currentInstance = Date.now();
        runningInstance = currentInstance;

        if (canvas) {
            while (canvas.width != width || canvas.height != height) {
                if (currentInstance != runningInstance) break;

                await sleep(0)

                requestAnimationFrame(() => {
                    if (canvas.width != width) {
                        const diffWidth = Math.abs(canvas.width - width);
                        if (canvas.width > width) {
                            canvas.width -= diffWidth > 5 ? 5 : 1;
                        } else if (canvas.width < width) {
                            canvas.width += diffWidth > 5 ? 5 : 1;
                        }
                    }
                    if (canvas.height != height) {
                        const diffHeight = Math.abs(canvas.height - height);
                        if (canvas.height > height) {
                            canvas.height -= diffHeight > 5 ? 5 : 1;
                        } else if (canvas.height < height) {
                            canvas.height += diffHeight > 5 ? 5 : 1;
                        }
                    }
    
                    animator.drawNow()
                })
            }
        }
    }, 100);

    const handlePointer = (ev: any) => {
        const { x, y, view } = ev;
        const { width, height } = view;
        const canvas = self.document.getCanvas();

        if (!canvas) return;

        const normalizedX = x / width * canvas.width;
        const normalizedY = y / height * canvas.height;

        animator.addHeart(normalizedX, normalizedY);
    }

    self.window.addEventListener('resize', handleResize);
    self.window.addEventListener('pointermove', handlePointer);
    self.window.addEventListener('pointerdown', handlePointer);
};
// Event Handler Callback
eventHandlers[IPCMessageType.EventHandlerCallback] = (message) => {
    let eventHandler: {
        id: number;
        type: string;
        event: any;
    } = <any>message;
    eventHandler.event.preventDefault = () => { };

    const func = IPCProxy.Get(eventHandler.id);
    if (func) {
        func(eventHandler.event);
    }
};

// Register Event Handler for all Worker Messages
class WorkerMessageEventHandler extends EventTarget {
    constructor() {
        super()
        for (let [key, value] of Object.entries(eventHandlers)) {
            this.addEventListener(IPCMessageType[parseInt(key)], (event: any) => {
                const customEvent: CustomEvent<IPCMessageDataType> = event;
                value(customEvent.detail);
            });
        }
    }
    public OnMessage = (type: IPCMessageType, message: IPCMessageDataType) => this.dispatchEvent(new CustomEvent<IPCMessageDataType>(IPCMessageType[type], { detail: message }));
    public static Handler = new WorkerMessageEventHandler();
}

// Worker TypeScript Def and boilerplate
interface IWorker extends Worker {
    window: WorkerDOM.Window;
    screen: unknown;
    document: WorkerDOM.Document;
}
declare let self: IWorker;

self.window = new WorkerDOM.Window();
self.screen = {};
self.onmessage = (ev: MessageEvent<IPCMessage>) => {
    if (ev.data !== undefined && ev.data.type !== undefined && ev.data.message !== undefined) {
        WorkerMessageEventHandler.Handler.OnMessage(ev.data.type, ev.data.message);
    }
};

export { };