
  <h3 align="center">t.Like</h3>

  ## About The Project

  Dating app: Bot, Web app, API. 

  ### Built With

* React
* TailwindCSS
* C#
* EntityFrameworkCore
* PostgreSQL
    ### Run With
* Docker
* Nginx
* Let`sEncrypt

### INSTRUCTIONS for 'clear' Ubuntu

```apt-get update
apt-get install nginx 
apt-get install snapd
snap install –classic certbot
ls -s /snap/bin/certbot /usr/bin/certbot
certbot –nginx
apt-get install nodejs
apt-get install npm
npm cache clean -f
npm install -g n
n stable
apt-get install –reinstall nodejs-legacy```

#### Relogin

```git clone https://github.com/Ooburi/Tinderlike.git
cd Tinderlike/React\ SPA/src/services/
nano ProfileServices.js```

#### Change {DOMAIN} with your actual domain

```cd ..
cd ..
npm run build
cd
mv Tinderlike/React\ SPA/dist/* /usr/share/nginx/html/
cd Tinderlike/
nano .env```

#### Set {BOT_TOKEN} with your bot_token

```nano nginx.conf```

#### Change {DOMAIN} with your actual domain

```cd
mv Tinderlike/nginx.conf /etc/nginx/sites-enabled/default
systemctl restart nginx
cd Tinderlike
docker-compose up -d\```

# Then set {DOMAIN} in settings of your bot for menu button and set webhook to https://{DOMAIN}/update
