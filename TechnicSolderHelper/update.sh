#!/bin/sh

cd $HOME
wget -c "http://zlepper.dk/solderhelper/TechnicSolderHelper.zip"
mkdir "SolderHelper"
unzip -o "TechnicSolderHelper.zip" -d "SolderHelper"
rm "TechnicSolderHelper.zip"
chmod 777 "SolderHelper/TechnicSolderHelper/SolderHelper.desktop"
chmod 777 "SolderHelper/TechnicSolderHelper/Run SolderHelper.sh"