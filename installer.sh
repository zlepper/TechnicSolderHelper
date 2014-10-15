sudo -s
sudo apt-get install zip unzip wine
wget -c "http://zlepper.dk/solderhelper/setup.exe"
wine setup.exe
wget -c "http://download.mono-project.com/repo/xamarin.gpg"
sudo apt-key add xamarin.gpg
sudo echo "deb http://download.mono-project.com/repo/debian wheezy main" > /etc/apt/sources.list.d/mono-xamarin.list
sudo echo "deb http://download.mono-project.com/repo/debian wheezy-apache24-compat main" >> /etc/apt/sources.list.d/mono-xamarin.list

