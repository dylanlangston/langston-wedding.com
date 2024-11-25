import { Button, Card, TextField, Typography } from '@mui/material';
import * as React from 'react';
import { useTranslation } from 'react-i18next';

interface GetInContactProps {

}

const GetInContact: React.FC<GetInContactProps> = ({ }) => {
  const { t } = useTranslation();

  

  return (
    <Card
      sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        p: {
            xs: 2,
            sm: 4
        },
        m: {
            xs: 0,
            sm: 'auto'
        },
        maxWidth: '600px'
      }}
    >
      <Typography sx={{fontWeight: "bold", alignSelf: 'flex-start'}} variant="h3" component="h1" gutterBottom>
        {t('Get in Touch')}
      </Typography>
      <TextField
        label={t("Full name")}
        variant="outlined"
        fullWidth
        sx={{ mb: 2 }}
      />
      <TextField
        label={t("Email address *")}
        variant="outlined"
        fullWidth
        sx={{ mb: 2 }}
      />
      <TextField
        label={t("Message")}
        variant="outlined"
        multiline
        rows={4}
        fullWidth
        sx={{ mb: 2 }}
      />
      <Button
        variant="contained"
        color="primary"
        sx={{ alignSelf: 'flex-end' }}
      >
        {t("Send")}
      </Button>
    </Card>
  );
};

export default GetInContact;