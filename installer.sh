#!/bin/sh

command -v mono >/dev/null 2>&1 || { echo >&2 "I require mono but it's not installed. Install it using your distros package manager!  Aborting."; exit 1; }
cd $HOME
echo "Downloading SolderHelper..."
wget -c "http://zlepper.dk/solderhelper/TechnicSolderHelper.zip" #Download archive
trap "echo 'Cleaning up...'; rm 'TechnicSolderHelper.zip'" EXIT #Set up trap to clean up temporary file
echo "Extracting Archive..."
unzip -o "TechnicSolderHelper.zip" #Extract (Will be in "TechnicSolderHelper")
echo "Making shortcut executable..."
chmod +x "TechnicSolderHelper/SolderHelper.desktop" #Setting executable bit on the shortcut file
