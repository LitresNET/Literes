FROM node:20-alpine AS build

WORKDIR /app

COPY package.json package-lock.json ./

RUN npm i --verbose

COPY . .

RUN npm run build --verbose
EXPOSE 8081
RUN chown -R node /app
USER node
CMD ["npm", "run", "preview"]