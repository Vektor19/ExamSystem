export class NoValidRequestError extends Error {
    constructor(message: string = "No valid request provided.") {
        super(message);
        this.name = "NoValidRequestError";
        Object.setPrototypeOf(this, NoValidRequestError.prototype);
    }
}