SHELL=/bin/bash

# Run 3 jobs in parallel
MAKEFLAGS += -j3

help: ## Display the help menu.
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-30s\033[0m %s\n", $$1, $$2}'

develop: develop-frontend run-azurite develop-dotnet

develop-frontend: # Develop frontend
	@npm run dev --prefix ./Frontend

run-azurite:
	@azurite --silent --loose --location ./azurite --debug ./azurite/debug.log

develop-dotnet:
	@cd Backend/Function;dotnet clean;dotnet watch

develop-backend: run-azurite develop-dotnet # Develop Backend

test: clean ## Default Test Target.
	@npm run test --prefix ./Frontend

release: clean  ## Default Release Target. Builds Web Version for publish
	@dotnet publish -c Release ./Backend/Function -o ./publish
	@cd publish; mkdir out; zip -r ./out/functionapp.zip .; cd ..
	@npm run build --prefix ./Frontend

setup: setup-node setup-dotnet ## Default Setup Target.

clean: ## Default Clean Target.
	@rm -rf ./Frontend/dist
	@dotnet clean ./Backend/
	@echo Cleaned Output

setup-node: # node Install
	@echo "-NodeJS-"
	@npm ci --prefix ./Frontend

setup-dotnet: # dotnet Install
	@echo "-dotnet-"
	@dotnet restore --locked-mode ./Backend

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
	@outdated_packages=$$(dotnet list ./Backend/Function package --outdated | awk '/^   > / {print $$2}'); \
	if [ -n "$$outdated_packages" ]; then \
	    echo "$$outdated_packages" | xargs -n 1 dotnet add ./Backend/Function package; \
	fi

swagger: ## Generate swagger docs
	@dotnet build --property DefineConstants="GENERATE_SWAGGER" ./Backend/Function
	@cd ./Backend/Function/; dotnet ./bin/Debug/net9.0/Function.dll
	@npm run swagger --prefix ./Frontend