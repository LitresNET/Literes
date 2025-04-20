export interface IPublishEndpoint {
    publish(message: any, messageType: any, ctx?: any): Promise<void>;
}