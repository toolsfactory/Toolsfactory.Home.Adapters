#! /bin/sh

if [ `whoami` != root ]; then
    echo Please run this script as root or using sudo
    exit
fi

SERVICE=d0powermeter
SERVICEDIR="/usr/share/oh${SERVICE}"
CONFIGDIR="/etc/ohadapters/${SERVICE}"


echo "Service installer: $SERVICE"
echo "--------------------------"
echo
echo "* jumping to install folder"
DIR=$(dirname $(readlink -f $0))
echo "  = $DIR"
cd $DIR

echo "* removing al x flags"
chmod -x *

echo "* checking if service exists"
if systemctl list-units --full -all | grep -Fq "$SERVICE.service"; then
  service $SERVICE stop
fi

echo "* checking if ${SERVICEDIR} exists"
if [ -d "$SERVICEDIR" ]; then
  # Take action if $DIR exists. #
  echo "* deleting old version"
  rm -rf  $SERVICEDIR
fi

echo "* checking if ${CONFIGDIR} exists"
if [ ! -d $CONFIGDIR ]; then
  mkdir -p $CONFIGDIR
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
mkdir -p $SERVICEDIR
mv * $SERVICEDIR
chmod -x $SERVICEDIR/*
chmod +x $SERVICEDIR/$SERVICE.host

echo "* enabling the service"
systemctl disable $SERVICE.service
systemctl enable $SERVICE.service
systemctl daemon-reload

while true; do
    read -p "==> Do you want to start the service?" yn
    case $yn in
        [Yy]* ) echo "* start the service"; service $SERVICE start; break;;
        [Nn]* ) break;;
            * ) echo "==> lease answer yes or no.";;
    esac
done

echo "done."