import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Card, Link } from '@mui/material';

interface Registry {
  id: number;
  registry: string;
  website: string;
  details: string;
}

const columns: GridColDef[] = [
  { field: 'registry', headerName: 'Registry', width: 150 },
  {
    field: 'website',
    headerName: 'Website',
    width: 150,
    renderCell: (params) => (
      <Link href={`https://${params.value}`} target="_blank" rel="noopener noreferrer">
        {params.value}
      </Link>
    ),
  },
  { field: 'details', headerName: 'Details', width: 200 },
];

const rows: Registry[] = [
  { id: 1, registry: 'Amazon', website: 'Amazon.com', details: 'Use code "langston"' },
  { id: 2, registry: 'Eddie Bauer', website: 'EddieBauer.com', details: 'Use code "langston"' },
  { id: 3, registry: 'Walmart', website: 'Walmart.com', details: 'N/A' },
  { id: 4, registry: 'Ikea', website: 'Ikea.com', details: 'Use code "langston"' },
  { id: 5, registry: 'Chewy', website: 'Chewy.com', details: 'N/A' },
];

const Registries = () => {
  return (
    <Card sx={{ width: '100%',
      maxWidth: {
          md: '70vw'
      },
      m: 'auto',
      "& .MuiDataGrid-cell:focus-within, & .MuiDataGrid-cell:focus": {
      outline: "none"
    } }}>
      <DataGrid
        rows={rows}
        columns={columns}
        checkboxSelection={false} // Disable checkbox selection if not needed
        rowSelection={false}
        disableColumnSelector={false}
        hideFooter={true}
        disableColumnFilter={true}
        disableColumnMenu={true}
      />
    </Card>
  );
}

export default Registries;