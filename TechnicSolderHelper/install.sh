#!/bin/sh

DISTRO=$(lsb_release -a | sed -n -e 's/^.*ID://p')
DISTRO=${DISTRO//[[:blank:]]/}

echo "$DISTRO detected."

function dl() {
	echo "Downloading SolderHelper..."
	wget -c "$1" >/dev/null 2>&1 #Download archive
	echo "Done."
}

function extract() {
	echo "Extracting Archive..."
	unzip -o "$1" >/dev/null 2>&1 #Extract (Will be in "TechnicSolderHelper")
	echo "Done."
}

function makexec() {
	echo "Marking $1 executable..."
	chmod +x $1
	echo "Done."
}

function install() {
	echo "Trying to install $1, you might get asked for your password."
	case "$DISTRO" in
		"Arch")
			sudo pacman -Sy $1
			;;
		"Debian" | "Ubuntu")
			if [[ $1 == "mono" ]]; then
				wget -c "http://download.mono-project.com/repo/xamarin.gpg"
				sudo apt-key add xamarin.gpg
				rm xamarin.gpg
				sudo echo "deb http://download.mono-project.com/repo/debian wheezy main" > /etc/apt/sources.list.d/mono-xamarin.list
				sudo echo "deb http://download.mono-project.com/repo/debian wheezy-apache24-compat main" >> /etc/apt/sources.list.d/mono-xamarin.list
				sudo apt-get -y update
				sudo apt-get -y install mono-complete
			else
				sudo apt-get -y install $1
			fi
			;;
		*)
			echo "Your package manager is not yet supported! Please install $1 yourself."
			;;
	esac
}

command -v zip >/dev/null 2>&1 || { install "zip"; }
command -v unzip >/dev/null 2>&1 || { install "unzip"; }
command -v mono >/dev/null 2>&1 || { install "mono"; }

dl "http://zlepper.dk/solderhelper/TechnicSolderHelper.zip"
trap "rm 'TechnicSolderHelper.zip'" EXIT
extract "TechnicSolderHelper.zip"
makexec "TechnicSolderHelper/SolderHelper.desktop"

echo "Successfully installed SolderHelper if no errors occured!"