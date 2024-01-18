export function NoAwait<T>(promise: Promise<T>): void {
    promise
        .catch(e => console.error(e));
}
