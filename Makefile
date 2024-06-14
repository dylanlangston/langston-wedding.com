SHELL=/bin/bash

help: ## Display the help menu.
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-30s\033[0m %s\n", $$1, $$2}'

develop: ## Default Developer Target
	@npm run dev --prefix ./Frontend

test: clean ## Default Test Target.
	@npm run test --prefix ./Frontend

release: clean  ## Default Release Target. Builds Web Version for publish
	@npm run develop --prefix ./Frontend

setup: setup-node ## Default Setup Target.

clean: ## Default Clean Target.
	@rm -rf ./Frontend/dist
	@echo Cleaned Output

setup-node: # node Install
	@npm install --prefix ./Frontend