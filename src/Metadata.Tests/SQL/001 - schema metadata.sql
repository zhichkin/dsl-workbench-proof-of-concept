USE [one-c-sharp];

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'ocs')
BEGIN
    -- The schema must be run in its own batch!
    EXECUTE('CREATE SCHEMA [ocs];');
END