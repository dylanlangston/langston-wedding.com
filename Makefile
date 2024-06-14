SHELL=/bin/bash

help: ## Display the help menu.
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-30s\033[0m %s\n", $$1, $$2}'

develop: ## Default Developer Target
	@bash ./develop.sh

develop-node: # develop node
	@npm run dev --prefix ./Frontend

develop-dotnet: # develop dotnet
	@cd Backend/Contact;func start --dotnet-isolated

test: clean ## Default Test Target.
	@npm run test --prefix ./Frontend

release: clean  ## Default Release Target. Builds Web Version for publish
	@dotnet publish -c Release ./Backend
	@npm run build --prefix ./Frontend

setup: setup-node setup-dotnet ## Default Setup Target.

clean: ## Default Clean Target.
	@rm -rf ./Frontend/dist
	@dotnet clean ./Backend/
	@echo Cleaned Output

setup-node: # node Install
	@echo "-NodeJS-"
	@npm install --prefix ./Frontend

setup-dotnet: # dotnet Install
	@echo "-dotnet-"
	@dotnet restore ./Backend

upgrade: # For when I make a typo
	@echo "It's 'make update'..."
	@make update

update: update-node update-dotnet ## Default Update Target.

update-node: # node update
	@echo "-NodeJS-"
	@npm update
	@npm upgrade

update-dotnet: # dotnet update
	@echo "-dotnet-"
	@outdated_packages=$$(dotnet list ./Backend/Contact package --outdated | awk '/^   > / {print $$2}'); \
	if [ -n "$$outdated_packages" ]; then \
	    echo "$$outdated_packages" | xargs -n 1 dotnet add ./Backend/Contact package; \
	fi