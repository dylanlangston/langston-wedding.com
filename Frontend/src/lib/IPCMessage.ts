import { hash } from "./hash";

export enum IPCMessageType {
    Initialize,
    Initialized,
    AddEventHandler,
    RemoveEventHandler,
    EventHandlerCallback,
    AddMediaQueryHandler,
    RemoveMediaQueryHandler
}

// This class is used for communication between web worker and main page
export class IPCMessage {
    public static Initialize = (canvas: OffscreenCanvas) => new IPCMessage(IPCMessageType.Initialize, {
        canvas,
    });
    public static Initialized = () => new IPCMessage(IPCMessageType.Initialized);
    public static AddEventHandler = (eventInfo: {
        id: number;
        target: string;
        type: string;
    }) => new IPCMessage(IPCMessageType.AddEventHandler, eventInfo);
    public static RemoveEventHandler = (eventInfo: {
        id: number;
        target: string;
        type: string;
    }) => new IPCMessage(IPCMessageType.RemoveEventHandler, eventInfo);
    public static EventHandlerCallback = (eventInfo: {
        id: number;
        target: string;
        type: string;
        event: any;
    }) => new IPCMessage(IPCMessageType.EventHandlerCallback, eventInfo);
    public static AddMediaQueryHandler = (eventInfo: {
        id: number;
        target: string;
        type: string;
    }) => new IPCMessage(IPCMessageType.AddMediaQueryHandler, eventInfo);
    public static RemoveMediaQueryHandler = (eventInfo: {
        id: number;
        target: string;
        type: string;
    }) => new IPCMessage(IPCMessageType.RemoveMediaQueryHandler, eventInfo);

    private constructor(type: IPCMessageType, message: IPCMessageDataType = undefined) {
        this.type = type;
        this.message = message;
    }
    public readonly type: IPCMessageType;
    public readonly message: IPCMessageDataType;

    public hash(): number {
        switch (this.type) {
            case IPCMessageType.Initialize:
                return hash(this.type.toString() + (<{
                    canvas: OffscreenCanvas,
                    audioSampleRate: number
                }>this.message).audioSampleRate);
            case IPCMessageType.EventHandlerCallback:
            case IPCMessageType.AddEventHandler:
            case IPCMessageType.RemoveEventHandler:
            case IPCMessageType.AddMediaQueryHandler:
            case IPCMessageType.RemoveMediaQueryHandler:
                const message = (<{
                    id: number;
                    target: string;
                    type: string;
                    event?: any;
                }>this.message);
                return hash(this.type.toString() + message.target + message.type + message.id);
            default:
                return hash(this.type.toString());
        };
    }
}

export type IPCMessageDataType =
    {
        canvas: OffscreenCanvas
    } |
    {
        id: number;
        target: string;
        type: string;
        event?: any;
    } |
    PointerEvent |
    undefined;