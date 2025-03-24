CREATE TABLE IF NOT EXISTS public."CashFlows" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL,
    "FlowDesc" TEXT NOT NULL,
    "FlowAmount" NUMERIC NOT NULL,
    "FlowDate" TIMESTAMP WITH TIME ZONE NOT NULL,
    "CatId" INTEGER,
    "PayId" INTEGER,
    "FlowType" VARCHAR(10) NOT NULL DEFAULT 'income',
    CONSTRAINT "FK_CashFlows_Categories" FOREIGN KEY ("CatId") REFERENCES public."Categories" ("Id"),
    CONSTRAINT "FK_CashFlows_PayWays" FOREIGN KEY ("PayId") REFERENCES public."PayWays" ("Id"),
    CONSTRAINT "FK_CashFlows_Users" FOREIGN KEY ("UserId") REFERENCES public."Users" ("Id") ON DELETE CASCADE
);