Vagrant.configure("2") do |config|

  config.vm.box = 'bento/ubuntu-22.04'
  config.vm.hostname = "dormcare-arm"

  config.vm.synced_folder ".", "/vagrant"

  config.vm.network "forwarded_port", guest: 2200, host: 2200

  config.vm.provider "virtualbox" do |vb|
    vb.memory = "4096"
    vb.cpus = 2
  end

  config.vm.provision "shell", path: "scripts/vagrant_provision.sh"
end
