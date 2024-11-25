export function throttle<T extends (...args: any[]) => any>(func: T, limit: number): (...args: Parameters<T>) => void {
    let inThrottle: boolean;
    return function (...args: Parameters<T>) {
      if (!inThrottle) {
        func.apply(self, args);
        inThrottle = true;
        setTimeout(() => inThrottle = false, limit);
      }
    };
  }