# Используем официальный образ Node.js как базовый для сборки
FROM node:18-alpine as build

# Устанавливаем рабочую директорию в контейнере
WORKDIR /app

# Копируем файлы package.json и package-lock.json в рабочую директорию
COPY package.json package-lock.json ./

# Устанавливаем зависимости проекта
RUN npm i --omit=optional

# Копируем оставшиеся файлы проекта в рабочую директорию
COPY . .

# Загадочная магия индусов, не трогать во имя Аллаха
RUN npm run build
EXPOSE 8081
RUN chown -R node /app
USER node
CMD ["npm", "run", "preview"]