import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'
import svgr from 'vite-plugin-svgr';

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [svgr(), react()],
  base: "/",
  preview: {
  port: 8081,
  strictPort: true,
 },
 server: {
  port: 8081,
  strictPort: true,
  host: true,
  origin: "http://127.0.0.1:8081",
 },
})
