import { IPCMessage } from "./IPCMessage";
import { IPCProxy } from "./IPCProxy";

// Minimum implementation of the DOM needed to make Emscripten run from our web worker
export namespace WorkerDOM {
    export class Window {
        public addEventListener(type: string, listener: EventListenerOrEventListenerObject, options?: boolean | AddEventListenerOptions): void {
            const id = IPCProxy.Add(listener, type);
            postMessage(IPCMessage.AddEventHandler({ id, target: 'Window', type }));
        }
        public removeEventListener(type: string, listener: EventListenerOrEventListenerObject, options?: boolean | AddEventListenerOptions): void {
            const id = IPCProxy.Remove(listener, type);
            postMessage(IPCMessage.RemoveEventHandler({ id, target: 'Window', type }));
        }
        public matchMedia(query: string): MediaQueryList {
            return <any>{
                addEventListener: (type: string, listener: EventListenerOrEventListenerObject, options?: boolean | AddEventListenerOptions): void => { 
                    const id = IPCProxy.Add(listener, type);
                    postMessage(IPCMessage.AddMediaQueryHandler({ id, target: query, type }));
                },
                removeEventListener: (type: string, listener: EventListenerOrEventListenerObject, options?: boolean | AddEventListenerOptions): void => { 
                    const id = IPCProxy.Remove(listener, type);
                    postMessage(IPCMessage.RemoveMediaQueryHandler({ id, target: query, type }));
                },
            };
        }
    }
    export class Document {
        private canvas: IOffscreenCanvasExtended | null;
        constructor(canvas: IOffscreenCanvasExtended | null) {
            this.canvas = canvas;
        }
        public addEventListener(type: string, listener: EventListenerOrEventListenerObject, options?: boolean | AddEventListenerOptions): void {
            const id = IPCProxy.Add(listener, type);
            postMessage(IPCMessage.AddEventHandler({ id, target: 'Document', type }));
        }
        public removeEventListener(type: string, listener: EventListenerOrEventListenerObject, options?: boolean | AddEventListenerOptions): void {
            // Skip resize events
            if (type == "resize") return;
            const id = IPCProxy.Remove(listener, type);
            postMessage(IPCMessage.RemoveEventHandler({ id, target: 'Window', type }));
        }
        public querySelector(selectors: string): HTMLElement | null {
            return <any>this.canvas;
        }
        public getCanvas = () => this.canvas;
        public body = new Body();
    }
    export class Body {
    }

    interface IOffscreenCanvasExtended extends OffscreenCanvas {
        clientWidth: number;
        clientHeight: number;
        getBoundingClientRect: () => {
            x: number;
            y: number;
            width: number;
            height: number;
            top: number;
            bottom: number;
            left: number;
        };
        dispatchEvent(event: Event): boolean;
        addEventListener: (type: string, listener: EventListenerOrEventListenerObject, options?: boolean | AddEventListenerOptions) => void;
        removeEventListener: (type: string, listener: EventListenerOrEventListenerObject, options?: boolean | AddEventListenerOptions) => void;
        height: number;
        oncontextlost: ((this: OffscreenCanvas, ev: Event) => any) | null;
        oncontextrestored: ((this: OffscreenCanvas, ev: Event) => any) | null;
        width: number;
        convertToBlob(options?: ImageEncodeOptions): Promise<Blob>;
        getContext(contextId: "2d", options?: any): OffscreenCanvasRenderingContext2D | null;
        getContext(contextId: "bitmaprenderer", options?: any): ImageBitmapRenderingContext | null;
        getContext(contextId: "webgl", options?: any): WebGLRenderingContext | null;
        getContext(contextId: "webgl2", options?: any): WebGL2RenderingContext | null;
        getContext(contextId: OffscreenRenderingContextId, options?: any): OffscreenRenderingContext | null;
        transferToImageBitmap(): ImageBitmap;
    }
    export class OffscreenCanvasExtended implements IOffscreenCanvasExtended {
        private canvas: OffscreenCanvas;
        constructor(canvas: OffscreenCanvas) {
            this.canvas = canvas;
        }
        public get clientWidth(): number {
            return this.canvas.width;
        }
        public get clientHeight(): number {
            return this.canvas.height;
        }
        public getBoundingClientRect: () => {
            x: number;
            y: number;
            width: number;
            height: number;
            top: number;
            bottom: number;
            left: number;
        } = () => {
            return { x: 0, y: 0, width: this.canvas.width, height: this.canvas.height, top: 0, right: this.canvas.width, bottom: this.canvas.height, left: 0 };
        }
        public dispatchEvent(event: Event): boolean {
            return this.canvas.dispatchEvent(event);
        }
        public addEventListener(type: string, listener: EventListenerOrEventListenerObject, options?: boolean | AddEventListenerOptions): void {
            const id = IPCProxy.Add(listener, type);
            postMessage(IPCMessage.AddEventHandler({ id, target: 'Canvas', type }));
        }
        public removeEventListener(type: string, listener: EventListenerOrEventListenerObject, options?: boolean | AddEventListenerOptions): void {
            const id = IPCProxy.Remove(listener, type);
            postMessage(IPCMessage.RemoveEventHandler({ id, target: 'Canvas', type }));
        }
        public set oncontextlost(value: ((this: OffscreenCanvas, ev: Event) => any) | null) {
            this.canvas.oncontextlost = value;
        }
        public get oncontextlost(): ((this: OffscreenCanvas, ev: Event) => any) | null {
            return this.canvas.oncontextlost;
        }
        public set oncontextrestored(value: ((this: OffscreenCanvas, ev: Event) => any) | null) {
            this.canvas.oncontextrestored = value;
        }
        public get oncontextrestored(): ((this: OffscreenCanvas, ev: Event) => any) | null {
            return this.canvas.oncontextrestored;
        }
        public set height(value: number) {
            this.canvas.height = value;
        }
        public get height(): number {
            return this.canvas.height;
        }
        public set width(value: number) {
            this.canvas.width = value;
        }
        public get width(): number {
            return this.canvas.width;
        }
        public convertToBlob(options?: ImageEncodeOptions): Promise<Blob> {
            return this.canvas.convertToBlob(options);
        }
        public getContext(contextId: any, options?: any): any {
            return this.canvas.getContext(contextId, options);
        }
        public transferToImageBitmap(): ImageBitmap {
            return this.canvas.transferToImageBitmap();
        }
    }
};