#! /bin/sh

if [ `whoami` != root ]; then
    echo Please run this script as root or using sudo
    exit
fi

SERVICE=Gasprices
SERVICEDIR="/usr/share/homie-${SERVICE}"
CONFIGDIR="/etc/homie-adapters/${SERVICE}"


echo "Service installer starting"
echo "--------------------------"
echo

echo "* jumping to install folder"
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
cd $DIR

echo "* removing al x flags"
chmod -x *

echo "* checking if service exists"
if systemctl list-units --full -all | grep -Fq "$SERVICENAME.service"; then
  service $SERVICENAME stop
fi

echo "* checking if ${SERVICEDIR} exists"
if [ -d "$SERVICEDIR" ]; then
  # Take action if $DIR exists. #
  echo "* deleting old version"
  rm -rf  $SERVICEDIR
fi

echo "* checking if ${CONFIGDIR} exists"
if [ ! -d $CONFIGDIR ]; then
  mkdir $CONFIGDIR
  mv -f appsettings* $CONFIGDIR
else
    echo "* checking if configuration files exist"
    if [ -e $CONFIGDIR/appsettings.json ]; then
      while true; do
        read -p "==> Do you want to override existing settings?" yn
        case $yn in
            [Yy]* ) mv -f appsettings* $CONFIGDIR; break;;
            [Nn]* ) rm appsettings*; break;;
             * ) echo "==> lease answer yes or no.";;
        esac
      done
    fi
fi

echo "* moving service file to /usr/lib/systemd/system"
mv $SERVICE.service /usr/lib/systemd/system

echo "* creating service directory at ${SERVICEDIR} and moving files there"
mkdir $SERVICEDIR
mv * $SERVICEDIR
chmod -x $SERVICEDIR/*
chmod +x $SERVICEDIR/$SERVICE.host

echo "* enabling the service"
systemctl disable $SERVICE.service
systemctl enable $SERVICE.service
systemctl daemon-reload

echo "* start the service"
service $SERVICENAME start