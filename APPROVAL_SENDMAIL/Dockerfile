FROM node:21-alpine

WORKDIR /usr/src/app
COPY ./package.json ./
COPY ./package-lock.json ./
RUN npm install

WORKDIR /usr/src/app/work
WORKDIR /usr/src/app/src
COPY ./src/App.css ./
COPY ./src/App.js ./
COPY ./src/index.css ./
COPY ./src/index.js ./
COPY ./src/logo.svg ./
COPY ./src/commons ./commons
COPY ./src/components ./components
COPY ./src/pages ./pages

WORKDIR /usr/src/app/ssl
COPY ./ssl ./

WORKDIR /usr/src/app
COPY ./node_modules ./
COPY ./public ./public
COPY ./.env ./

CMD ["npm", "start"]