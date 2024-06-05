export interface ChatbotQuery {
    text: string;
}

export interface ChatbotResponse {
    text: string;
}

export class ChatbotService {
    async processQuery(query: ChatbotQuery): Promise<ChatbotResponse> {
        return { text: `You said: ${query.text}` };
    }
}