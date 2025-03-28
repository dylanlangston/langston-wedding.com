@export()
func ipsToRuleArray(ipsString string, actionString string) array =>
  map(split(ipsString, ','), ip => {
    ipAddress: ip
    action: actionString
  })
