# Jeg kan ikke
Et API som finner på unnskyldninger for deg

## Endpoints
Root URL: https://api.jegkanikke.no/

- [GET] /excuses: Returns a list of Excuses
- [GET] /excuses/{id}: Returns the specified Excuse
- [POST] /excuses: Creates a new Excuse
- [PUT] /excuses/{id}: Updates a new Excuse
- [DELETE] /excuses/{id}: Deletes the specified Excuse

## Models
### Excuse
- id: string
- message: string