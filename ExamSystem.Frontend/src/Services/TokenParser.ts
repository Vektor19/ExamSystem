
class TokenParser {
    parseIdFromToken(token: string): string | null {
        try {
            const payload = token.split(".")[1];
            const decodedPayload = atob(payload.replace(/-/g, "+").replace(/_/g, "/"));
            const jsonPayload = JSON.parse(decodedPayload);
            return jsonPayload.sub || null;
        } catch (error) {
            console.error("Error parsing token:", error);
            return null;
        }
    }
}

export default new TokenParser();
