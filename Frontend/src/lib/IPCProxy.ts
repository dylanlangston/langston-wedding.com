import { hash } from "./hash";

// Essentially just a hash map
export class IPCProxy {
    private static proxies: { [type: number]: any | null } = {};
    public static Add(f: any, type: string,): number {
        const index = hash(f.toString() + type)
        this.proxies[index] = f;
        return index;
    }
    public static Remove = (f: any, type: string,): number => {
        const index = hash(f.toString() + type);
        this.proxies[index] = null;
        return index;
    }
    public static RemoveExisting = (f: any, type: string): any => {
        const index = hash(f.toString() + type);
        const temp = this.proxies[index];
        this.proxies[index] = null;
        return temp;
    }
    public static Get = (n: number): any | null => this.proxies[n];
    public static GetAndRemove = (n: number): any | null => {
        const temp = this.proxies[n];
        this.proxies[n] = null;
        return temp;
    }

}