CREATE
    OR REPLACE FUNCTION consolidate(accountId BIGINT) RETURNS VOID AS
$$
DECLARE
    matchingPrincipalCount INTEGER;
    deletedPrincipalCount  INTEGER;
    modifiedClueCount      INTEGER;
    selectedId             UUID;
    selectedAccountName    VARCHAR;
BEGIN
    RAISE NOTICE 'Start consolidation of account %...', accountId;

    matchingPrincipalCount := COUNT(*) FROM "Principals" WHERE "AccountId" = accountId;

    IF matchingPrincipalCount = 0 THEN RAISE NOTICE 'No registration for account with AccountId=%.', accountId; END IF;

    RAISE NOTICE 'Found % registrations for account with AccountId=%.', matchingPrincipalCount, accountId;

    SELECT "Id", "AccountName"
    INTO selectedId, selectedAccountName
    FROM "Principals"
    WHERE "AccountId" = accountId
    ORDER BY "RegistrationDate" DESC
    LIMIT 1;

    RAISE NOTICE 'Most recent registration found: Id: %, AccountId: %, AccountName:%.', selectedId, accountId, selectedAccountName;
    WITH UpdatedClueRecords AS
             (UPDATE "ClueRecords" SET "AuthorId" = selectedId
                 WHERE EXISTS (SELECT "Id"
                               FROM "Principals"
                               WHERE "Id" = "ClueRecords"."AuthorId"
                                 AND "AccountId" = accountId)
                 RETURNING *)
    SELECT COUNT(*)
    INTO modifiedClueCount
    FROM UpdatedClueRecords;

    RAISE NOTICE 'Updated a total of % clue records.', modifiedClueCount;

    WITH DeletedPrincipals AS
             (DELETE FROM "Principals"
                 WHERE "Id" != selectedId AND "AccountId" = accountId
                 RETURNING *)
    SELECT COUNT(*)
    INTO deletedPrincipalCount
    FROM DeletedPrincipals;

    RAISE NOTICE 'Deleted a total of % principals.', deletedPrincipalCount;
END;
$$ LANGUAGE plpgsql;

BEGIN;
SELECT consolidate(123456);
COMMIT;