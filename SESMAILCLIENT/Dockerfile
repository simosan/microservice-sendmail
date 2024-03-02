FROM node:21-alpine

WORKDIR /usr/src/app
COPY ./package.json ./
COPY ./package-lock.json ./
RUN npm install

WORKDIR /usr/src/app/work

WORKDIR /usr/src/app
COPY ./node_modules ./
COPY ./sendmail.js ./
COPY ./server.js ./
COPY ./.env ./

CMD ["npm", "start"]