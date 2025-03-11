import { Button, Card, TextField, Typography, CircularProgress } from '@mui/material';
import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { Contact } from '../lib/api/Contact';
import { API_URL } from '../utilities';

interface GetInContactProps {}

const api = new Contact({ baseUrl: API_URL });

const GetInContact: React.FC<GetInContactProps> = () => {
  const { t } = useTranslation();
  const [loading, setLoading] = React.useState(false);
  const [errors, setErrors] = React.useState<{ name?: string; email?: string; message?: string }>({});

  const onSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setErrors({});

    const target = event.currentTarget;
    const formData = new FormData(target);
    const name = formData.get('name') as string;
    const email = formData.get('email') as string;
    const message = formData.get('message') as string;

    let formErrors: typeof errors = {};
    if (!name) formErrors.name = t('Full name is required');
    if (!email) formErrors.email = t('Email is required');
    else if (!/^[^@\s]+@[^@\s]+\.[^@\s]+$/.test(email)) formErrors.email = t('Invalid email address');
    if (!message) formErrors.message = t('Message is required');

    if (Object.keys(formErrors).length > 0) {
      setErrors(formErrors);
      return;
    }

    setLoading(true);
    try {
      await api.contact({ name, email, message });
      alert(t('Message sent successfully!'));
      target?.reset();
    } catch (error) {
      debugger
      alert(t('Failed to send message. Please try again later.'));
    } finally {
      setLoading(false);
    }
  };

  return (
    <Card
      sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        p: { xs: 2, sm: 4 },
        m: { xs: 0, sm: 'auto' },
        maxWidth: '600px'
      }}
    >
      <Typography sx={{ fontWeight: "bold", alignSelf: 'flex-start' }} variant="h3" component="h1" gutterBottom>
        {t('Get in Touch')}
      </Typography>
      <form onSubmit={onSubmit} style={{ width: '100%' }}>
        <TextField
          name="name"
          label={t("Full name")}
          variant="outlined"
          fullWidth
          sx={{ mb: 2 }}
          error={!!errors.name}
          helperText={errors.name}
        />
        <TextField
          name="email"
          label={t("Email address *")}
          variant="outlined"
          fullWidth
          sx={{ mb: 2 }}
          error={!!errors.email}
          helperText={errors.email}
        />
        <TextField
          name="message"
          label={t("Message")}
          variant="outlined"
          multiline
          rows={4}
          fullWidth
          sx={{ mb: 2 }}
          error={!!errors.message}
          helperText={errors.message}
        />
        <Button
          type="submit"
          variant="contained"
          color="primary"
          sx={{ alignSelf: 'flex-end' }}
          disabled={loading}
        >
          {loading ? <CircularProgress size={24} /> : t("Send")}
        </Button>
      </form>
    </Card>
  );
};

export default GetInContact;
