//Added By IQSS as part of EDIMaintTool/Scheduler projects
var menuitems = {
    data: [
        {
            text: 'ADMINISTRATION', id: '0', expanded: false,
            items: [
                {
                    text: 'Document Look Up Data', id: '0', expanded: false,
                    items: [
                    { text: 'Manage Doc Types', LinksTo: './EDIDocumentType.aspx' },
                    { text: 'Manage Loops', LinksTo: './EDILoops.aspx' },
                    { text: 'Manage Segments', LinksTo: './EDISegment.aspx' },
                    ]
                },
                {
                    text: 'Master EDI Documents', id: '0', expanded: false,
                    items: [
                    { text: 'Manage Master Docs', LinksTo: './EDIMasterDocument.aspx' },
                    { text: 'Master Doc Configurations', LinksTo: './EDIMasterDocument.aspx' },
                    ]
                }
            ]
        },
        {
            text: 'Trading Partner EDI Documents', id: '0', expanded: false,
            items: [
                    { text: 'Manage TP Docs', LinksTo: './EDITPDocument.aspx' },
                    { text: 'TP Doc Configuration', LinksTo: './EDITPDocumentConfig.aspx' },
            ]
        },
        {
            text: 'Appointment Management System', id: '0', expanded: true,
            items: [
                    { text: 'Warehouse Scheduler', LinksTo: './Scheduler.aspx' },
                    { text: 'Manage Schedule', LinksTo: './ManageSchedule.aspx' },
                    { text: 'Carrier Scheduler', LinksTo: './CarrierScheduler.aspx' },
            ]
        }
    ]
};