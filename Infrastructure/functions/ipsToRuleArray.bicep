@export()
func ipsToRuleArray(ipsString string) array =>
  map(split(ipsString, ','), ip => {
    value: ip
    action: 'Allow'
  })
