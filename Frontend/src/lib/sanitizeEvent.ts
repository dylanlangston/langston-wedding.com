export const sanitizeEvent = <T>(e: any, n: number = 0): T => {
    const obj: any = {};

    for (let k in e) {
        if (e[k] == null || e[k] == undefined) continue;

        // Only go 6 levels deep
        if (n > 6) continue;

        if (e[k] instanceof Node) continue;
        if (e[k] instanceof Window) {
            obj[k] = {
                width: window.innerWidth,
                height: window.innerHeight,
                devicePixelRatio: window.devicePixelRatio
            };
            continue;
        }
        if (e[k] instanceof Array) {
            obj[k] = (obj[k] as Array<any>).map(o => sanitizeEvent(o[k], n + 1));
            continue;
        }
        if (e[k] instanceof Function) continue;

        switch (typeof e[k]) {
            case 'undefined':
            case 'boolean':
            case 'number':
            case 'bigint':
            case 'string':
                obj[k] = e[k];
                break;
            case 'object':
                try {
                    obj[k] = sanitizeEvent(e[k], n + 1);
                }
                catch {
                    continue;
                }
                break;
        }
    }
    return <T>obj;
}