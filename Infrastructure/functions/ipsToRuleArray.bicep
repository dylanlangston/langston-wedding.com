@export()
func ipsToRuleArray(ipsString string, actionString string) array =>
  map(split(ipsString, '\n'), ip => {
    ipAddress: ip
    action: actionString
  })
