import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
// @ts-ignore
import brotli from "rollup-plugin-brotli";
import zlib from "zlib";
import gzipPlugin from 'rollup-plugin-gzip';

// https://vitejs.dev/config/
export default defineConfig({
	plugins: [
		react(),
		//Brotli plugin with some defaults.
		brotli({
			test: /\.(js|css|html|txt|xml|json|svg)$/,
			options: {
				params: {
					[zlib.constants.BROTLI_PARAM_MODE]: zlib.constants.BROTLI_MODE_GENERIC,
					[zlib.constants.BROTLI_PARAM_QUALITY]: 11
				}
				// ... see all options https://nodejs.org/api/zlib.html#zlib_class_brotlioptions
			},

			// Ignore files smaller than this.
			//1000 is what cloudfront does: https://docs.aws.amazon.com/AmazonCloudFront/latest/DeveloperGuide/ServingCompressedFiles.html
			minSize: 1000
		}),

		//Gzip plugin with some defaults.
		gzipPlugin({
			gzipOptions: {
				level: 9,
			}
		})
	],
	server: {
		watch: {
			usePolling: true,
		},
		cors: true,
		host: true, // needed for the DC port mapping to work
		strictPort: true,
		port: 5173
	},
})
