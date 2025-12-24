# Vagrant (Ubuntu ARM)

```bash
# Execute provisioning (configuration and installation of dependencies)
vagrant provision

# Bring up the VM
vagrant up --provider=virtualbox

# View provisioning / app logs
vagrant ssh -- tail -n 200 /var/log/dormcare.log

# Stop the VM
vagrant halt

# Destroy the VM
vagrant destroy -f
```
