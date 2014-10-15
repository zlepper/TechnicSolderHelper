#!/bin/sh

sudo apt-get -y install zip unzip
wget -c "http://download.mono-project.com/repo/xamarin.gpg"
sudo apt-key add xamarin.gpg
rm xamarin.gpg
sudo echo "deb http://download.mono-project.com/repo/debian wheezy main" > /etc/apt/sources.list.d/mono-xamarin.list
sudo echo "deb http://download.mono-project.com/repo/debian wheezy-apache24-compat main" >> /etc/apt/sources.list.d/mono-xamarin.list
sudo apt-get -y update
sudo apt-get -y install mono-complete
cd $HOME
wget -c "http://zlepper.dk/solderhelper/TechnicSolderHelper.zip"
mkdir "SolderHelper"
unzip -o "TechnicSolderHelper.zip" -d "SolderHelper"
rm "TechnicSolderHelper.zip"
chmod 777 "SolderHelper/TechnicSolderHelper/SolderHelper.desktop"
