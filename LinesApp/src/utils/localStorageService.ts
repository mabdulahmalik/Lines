class LocalStorageService {
    private prefix: string;

    constructor(prefix: string) {
        this.prefix = prefix + '___';
    }

    setItem(key: string, value: string): void {
        localStorage.setItem(this.prefix + key, value);
    }

    getItem(key: string): string | null {
        return localStorage.getItem(this.prefix + key);
    }

    removeItem(key: string): void {
        localStorage.removeItem(this.prefix + key);
    }

    clear(): void {
        Object.keys(localStorage).forEach((key) => {
            if (key.startsWith(this.prefix)) {
                localStorage.removeItem(key);
            }
        });
    }
}

export default LocalStorageService;