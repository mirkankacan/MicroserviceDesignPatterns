apt-get install -y debian-keyring  # debian only
apt-get install -y debian-archive-keyring  # debian only
apt-get install -y apt-transport-https
# For Debian Stretch, Ubuntu 16.04 and later
keyring_location=/usr/share/keyrings/eventstore-eventstore-archive-keyring.gpg
# For Debian Jessie, Ubuntu 15.10 and earlier
keyring_location=/etc/apt/trusted.gpg.d/eventstore-eventstore.gpg
curl -1sLf 'https://packages.eventstore.com/public/eventstore/gpg.D008FDA5E151E345.key' |  gpg --dearmor >> ${keyring_location}
curl -1sLf 'https://packages.eventstore.com/public/eventstore/config.deb.txt?distro=ubuntu&codename=zorin&component=main' > /etc/apt/sources.list.d/eventstore-eventstore.list
sudo chmod 644 ${keyring_location}
sudo chmod 644 /etc/apt/sources.list.d/eventstore-eventstore.list
apt-get update