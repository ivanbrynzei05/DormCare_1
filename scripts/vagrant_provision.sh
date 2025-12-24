#!/usr/bin/env bash
set -euo pipefail

export DEBIAN_FRONTEND=noninteractive

echo "Provisioning VM: update and install prerequisites"
apt-get update -y
apt-get install -y curl wget git ca-certificates build-essential libssl-dev unzip

DOTNET_INSTALL_DIR=/usr/share/dotnet
mkdir -p "$DOTNET_INSTALL_DIR"
curl -sSL https://dot.net/v1/dotnet-install.sh -o /tmp/dotnet-install.sh
chmod +x /tmp/dotnet-install.sh

/tmp/dotnet-install.sh --channel 9.0 --install-dir "$DOTNET_INSTALL_DIR" --architecture arm64 || \
  /tmp/dotnet-install.sh --channel 7.0 --install-dir "$DOTNET_INSTALL_DIR" --architecture arm64

ln -sf "$DOTNET_INSTALL_DIR/dotnet" /usr/bin/dotnet || true

echo "Building and starting DormCare"
if [ -d /vagrant/DormCare ]; then
  cd /vagrant/DormCare

  if [ -f DormCareSolution.sln ]; then
    $DOTNET_INSTALL_DIR/dotnet restore DormCareSolution.sln
    $DOTNET_INSTALL_DIR/dotnet build DormCareSolution.sln -c Release
  elif [ -f DormCare.csproj ]; then
    $DOTNET_INSTALL_DIR/dotnet restore DormCare.csproj
    $DOTNET_INSTALL_DIR/dotnet build DormCare.csproj -c Release
  else
    echo "No solution or project file found in /vagrant/DormCare"
  fi


  nohup $DOTNET_INSTALL_DIR/dotnet run --project DormCare.csproj --urls "http://0.0.0.0:2200" &> /var/log/dormcare.log &
  echo "DormCare started (logs: /var/log/dormcare.log)"
else
  echo "Warning: /vagrant/DormCare not found. Ensure you run Vagrant from the project root."
fi
