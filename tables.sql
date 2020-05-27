CREATE TABLE [dbo].[Product] (
    [ProductCode]  VARCHAR (10)    NOT NULL,
    [ProductName]  VARCHAR (100)   NULL,
    [ProductPrice] DECIMAL (18, 2) NULL,
    [ProductImage] VARBINARY (MAX) NULL,
    [Uom]          VARCHAR (10)    NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([ProductCode] ASC)
);

CREATE TABLE [dbo].[Cart] (
    [ProductCode] VARCHAR (10) NOT NULL,
    [Qty]         INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([ProductCode] ASC),
    FOREIGN KEY ([ProductCode]) REFERENCES [dbo].[Product] ([ProductCode])
);

