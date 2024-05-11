# Используем официальный образ Node.js как базовый для сборки
FROM node:20-alpine AS build-node

# Устанавливаем рабочую директорию для Nest.js проекта
WORKDIR /app

# Копируем файлы package.json и package-lock.json
COPY ./package*.json ./

# Устанавливаем зависимости для Nest.js приложения
RUN npm --verbose install

# Копируем оставшиеся файлы Nest.js проекта
COPY . .

# Собираем Nest.js приложение
RUN npm run build

# Создаем образ Node.js для запуска приложения
FROM node:20-alpine AS runtime-node
WORKDIR /app

# Копируем собранные файлы Nest.js приложения
COPY --from=build-node /app/dist ./dist
COPY --from=build-node /app/node_modules ./node_modules

# Запускаем Nest.js приложение
CMD ["node", "dist/main.js"]