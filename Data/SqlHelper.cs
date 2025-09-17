using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Home.Data
{
    public class SqlHelper
    {
        private readonly string _connectionString;

        public SqlHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> CreatePropertyImageTableAsync()
        {
            try
            {
                // SQL script to create PropertyImage table if it doesn't exist
                string sql = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PropertyImage' AND schema_id = SCHEMA_ID('dbo'))
                BEGIN
                    CREATE TABLE [dbo].[PropertyImage] (
                        [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                        [PropertyId] INT NOT NULL,
                        [ImageUrl] NVARCHAR(500) NOT NULL,
                        [ThumbnailUrl] NVARCHAR(500) NULL,
                        [ImageTitle] NVARCHAR(200) NULL,
                        [ImageDescription] NVARCHAR(500) NULL,
                        [IsMainImage] BIT NOT NULL DEFAULT(0),
                        [DisplayOrder] INT NOT NULL DEFAULT(0),
                        [ImageType] NVARCHAR(100) NULL,
                        [ProcessedDate] DATETIME2 NULL,
                        [InsertedDate] DATETIME2 NOT NULL DEFAULT(GETDATE()),
                        [InsertedBy] NVARCHAR(100) NULL,
                        [UpdatedDate] DATETIME2 NULL,
                        [UpdatedBy] NVARCHAR(100) NULL,
                        [DeletedDate] DATETIME2 NULL,
                        [DeletedBy] NVARCHAR(100) NULL,
                        CONSTRAINT [FK_PropertyImage_Property_PropertyId] FOREIGN KEY ([PropertyId]) 
                            REFERENCES [dbo].[Property]([Id]) ON DELETE NO ACTION
                    )
                    PRINT 'PropertyImage table created successfully.'
                END
                ELSE
                BEGIN
                    PRINT 'PropertyImage table already exists.'
                END
                ";

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(sql, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating PropertyImage table: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CreatePropertyTableAsync()
        {
            try
            {
                // SQL script to create Property table if it doesn't exist
                string sql = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Property' AND schema_id = SCHEMA_ID('dbo'))
                BEGIN
                    CREATE TABLE [dbo].[Property] (
                        [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                        [Title] NVARCHAR(200) NOT NULL,
                        [TitleAr] NVARCHAR(200) NULL,
                        [Description] NVARCHAR(MAX) NULL,
                        [DescriptionAr] NVARCHAR(MAX) NULL,
                        [Address] NVARCHAR(500) NULL,
                        [AddressAr] NVARCHAR(500) NULL,
                        [Price] DECIMAL(18, 2) NOT NULL,
                        [Area] DECIMAL(18, 2) NOT NULL,
                        [Bedrooms] INT NULL,
                        [Bathrooms] INT NULL,
                        [Garages] INT NULL,
                        [YearBuilt] INT NULL,
                        [CountryId] INT NULL,
                        [CityId] INT NULL,
                        [DistrictId] INT NULL,
                        [AgentId] INT NOT NULL,
                        [RealStateTypeId] INT NOT NULL,
                        [RealStatePurposeId] INT NOT NULL,
                        [RealStateRentTypeId] INT NULL,
                        [FloorNumber] INT NULL,
                        [NumberOfFloors] INT NULL,
                        [IsFeatured] BIT NOT NULL DEFAULT(0),
                        [IsActive] BIT NOT NULL DEFAULT(1),
                        [ViewCount] INT NOT NULL DEFAULT(0),
                        [Features] NVARCHAR(MAX) NULL,
                        [FeaturesAr] NVARCHAR(MAX) NULL,
                        [Latitude] FLOAT NULL,
                        [Longitude] FLOAT NULL,
                        [InsertedDate] DATETIME2 NOT NULL DEFAULT(GETDATE()),
                        [InsertedBy] NVARCHAR(100) NULL,
                        [UpdatedDate] DATETIME2 NULL,
                        [UpdatedBy] NVARCHAR(100) NULL,
                        [DeletedDate] DATETIME2 NULL,
                        [DeletedBy] NVARCHAR(100) NULL,
                        CONSTRAINT [FK_Property_Country_CountryId] FOREIGN KEY ([CountryId]) 
                            REFERENCES [dbo].[Country]([Id]) ON DELETE NO ACTION,
                        CONSTRAINT [FK_Property_City_CityId] FOREIGN KEY ([CityId]) 
                            REFERENCES [dbo].[City]([Id]) ON DELETE NO ACTION,
                        CONSTRAINT [FK_Property_District_DistrictId] FOREIGN KEY ([DistrictId]) 
                            REFERENCES [dbo].[District]([Id]) ON DELETE NO ACTION,
                        CONSTRAINT [FK_Property_Agent_AgentId] FOREIGN KEY ([AgentId]) 
                            REFERENCES [dbo].[Agent]([Id]) ON DELETE NO ACTION,
                        CONSTRAINT [FK_Property_RealStateType_RealStateTypeId] FOREIGN KEY ([RealStateTypeId]) 
                            REFERENCES [dbo].[RealStateType]([Id]) ON DELETE NO ACTION,
                        CONSTRAINT [FK_Property_RealStatePurpose_RealStatePurposeId] FOREIGN KEY ([RealStatePurposeId]) 
                            REFERENCES [dbo].[RealStatePurpose]([Id]) ON DELETE NO ACTION,
                        CONSTRAINT [FK_Property_RealStateRentType_RealStateRentTypeId] FOREIGN KEY ([RealStateRentTypeId]) 
                            REFERENCES [dbo].[RealStateRentType]([Id]) ON DELETE NO ACTION
                    )
                    PRINT 'Property table created successfully.'
                END
                ELSE
                BEGIN
                    PRINT 'Property table already exists.'
                END
                ";

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(sql, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating Property table: {ex.Message}");
                return false;
            }
        }
    }
}