import path from "node:path";
import { generateApi } from 'swagger-typescript-api';

const options = {
    output: path.resolve(process.cwd(), './src//lib/api'),
    input: path.resolve(process.cwd(), '../Backend/src/Presentation/Functions/bin/Debug/net9.0/swagger.json'),
    modular: true,
    generateClient: true,
    cleanOutput: true,
    defaultResponseType: "undefined",
    responses: true,
    extractRequestBody: true,
    unwrapResponseData: true,
    generateUnionEnums: true,
    addReadonly: true,
    extractingOptions: {
        requestBodySuffix: ["Request", "Payload", "Body", "Input"],
        requestParamsSuffix: ["Params"],
        responseBodySuffix: ["Data", "Result", "Output"],
        responseErrorSuffix: [
            "Error",
            "Fail",
            "Fails",
            "ErrorData",
            "HttpError",
            "BadResponse",
        ],
    },
    primitiveTypeConstructs: (struct) => ({
        string: {
            "date-time": "Date",
            uuid: () => "uuidV4",
        },
    })
};

generateApi(options)
    .then(() => {
        console.log('API client generated successfully!');
    })
    .catch((error) => {
        console.error('Error generating API client:', error);
    });