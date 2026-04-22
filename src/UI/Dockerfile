FROM node:22.21 AS build

WORKDIR /app

COPY package.json package.json
#COPY package-lock.json package-lock.json

RUN npm i

COPY . .

RUN npm run build

FROM nginx:alpine

COPY --from=build /app/default.conf.template /etc/nginx/templates/default.conf.template
COPY --from=build /app/dist /usr/share/nginx/html

# Expose the default nginx port
EXPOSE 80

# Imposta la variabile per forzare il processing
ENV NGINX_ENVSUBST_OUTPUT_DIR=/etc/nginx/conf.d

CMD ["nginx", "-g", "daemon off;"]