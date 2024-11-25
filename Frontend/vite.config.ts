import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
// @ts-ignore
import brotli from "rollup-plugin-brotli";
import zlib from "zlib";
import gzipPlugin from 'rollup-plugin-gzip';
import copy from 'rollup-plugin-copy';

// https://vitejs.dev/config/
export default defineConfig({
	plugins: [
		react(),

		// Copy index.html to 404.html on build
		copy({
			targets: [
				{ src: 'dist/index.html', dest: 'dist', rename: '404.html' },
				{ src: 'dist/index.html.gz', dest: 'dist', rename: '404.html.gz' },
				{ src: 'dist/index.html.br', dest: 'dist', rename: '404.html.br' },
			],
			hook: 'closeBundle',
		}),

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
		}),
	],
	build: {
		rollupOptions: {
			output: {
				manualChunks: {
					'@mui': ['@emotion/react', '@emotion/styled', '@mui/material', '@mui/icons-material'],
					'react': ['react', 'react-dom', 'react-i18next', 'react-router-dom'],
				}
			}
		}
	},
	server: {
		watch: {
			usePolling: true,
		},
		proxy:
		{
			'/api/': {
				target: 'http://127.0.0.1:7071/',
			},
		},
		cors: false,
		host: true, // needed for the DC port mapping to work
		strictPort: true,
		port: 5173
	},
})
