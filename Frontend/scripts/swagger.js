import fs from "node:fs";
import path from "node:path";
import { generateApi } from 'swagger-typescript-api';

const options = {
    name: 'api.ts',
    output: path.resolve(process.cwd(), './src/api'),
    input: path.resolve(process.cwd(), '../Backend/Function/bin/Debug/net9.0/swagger.json'),
    generateClient: true,
    extractRequestBody: true,
    unwrapResponseData: true,
};

generateApi(options)
    .then(() => {
        console.log('API client generated successfully!');
    })
    .catch((error) => {
        console.error('Error generating API client:', error);
    });