# [short title of solved problem and solution]

* Status: [proposed | rejected | accepted | deprecated | … | superseded by [ADR-0005](0005-example.md)] <!-- optional -->
* Deciders: [list everyone involved in the decision] <!-- optional -->
* Date: [YYYY-MM-DD when the decision was last updated] <!-- optional -->

Technical Story: [description | ticket/issue URL] <!-- optional -->

## Context and Problem Statement

We designed and built our HR management system with multi-tenancy in mind. It was created to serve our company, and in the future other companies, to improve the hiring process.

Multi tenant architecture is an ecosystem or model, in which a single environment can serve multiple tenants utilizing a scalable, available, and resilient architecture. The underlying infrastructure is completely shared, logically isolated, and with fully centralized services. The multi tenant architecture evolves according to the organization or subdomain (organization.saas.com) that is logged into the SaaS application; and is totally transparent to the end-user.



![image](https://user-images.githubusercontent.com/10514279/136888798-5b46c4df-aa35-4e55-9e44-b51dba17fad9.png)

## Multi-tenant Benefits
*  A reduction of server Infrastructure costs utilizing a Multi tenant architecture strategy.
*  One single source of trust.
*  Cost reductions of development and time-to-market.

## Decision Drivers <!-- optional -->

* [driver 1, e.g., a force, facing concern, …]
* [driver 2, e.g., a force, facing concern, …]
* … <!-- numbers of drivers can vary -->

## Considered Options
The figure below shows three different database designs used for achieving multi-tenancy data architecture. Every approach has its own pros and cons.

* Dedicated Database
* Dedicated Table/Schema
* Shared Table/ Schema


![image](https://user-images.githubusercontent.com/10514279/136889463-d35d4a1c-ebd7-4518-8557-0f2c35f43073.png)


## Decision Outcome

Chosen option: "[option 1]", because [justification. e.g., only option, which meets k.o. criterion decision driver | which resolves force force | … | comes out best (see below)].

### Positive Consequences <!-- optional -->

* [e.g., improvement of quality attribute satisfaction, follow-up decisions required, …]
* …

### Negative Consequences <!-- optional -->

* [e.g., compromising quality attribute, follow-up decisions required, …]
* …

## Pros and Cons of the Options <!-- optional -->

### [Dedicated Database]

This approach allocates a new database for every new tenant. Separating tenant data in different databases is the simplest way of achieving isolation. It will allow you to extend the database of your choice if your software logic also allows for it. This database design tends to lead to higher costs for hardware and maintenance. You need to consider that you are also limited by the number of databases that the server can support. This solution is good for clients that have strong requirements for data isolation and also when the number of clients is not too large

[example | description | pointer to more information | …] <!-- optional -->

* Good, because [argument a]
* Good, because [argument b]
* Bad, because [argument c]
* … <!-- numbers of pros and cons can vary -->

### [Dedicated Schema]
This approach involves storing all tenants in a single database and separating every tenant by creating a different schema for its tables. In this way, each tenant will have its own set of tables within the same database. This database design keeps the hardware cost low by using the same database for all tenants.

[example | description | pointer to more information | …] <!-- optional -->

* Good, because [argument a]
* Good, because [argument b]
* Bad, because [argument c]
* … <!-- numbers of pros and cons can vary -->

### [Shared Table]
This approach involves storing all tenants’ data in the same database using the same schema for all tables. A special column is added to associate each record with its own tenant. Usually, that column is named TenantId and points to a specific tenant by using a foreign key. The hardware cost is still low, but you may need some additional development to ensure the tenant data is never accessed by another tenant.

[example | description | pointer to more information | …] <!-- optional -->

* Good, because [argument a]
* Good, because [argument b]
* Bad, because [argument c]
* … <!-- numbers of pros and cons can vary -->

## Links <!-- optional -->

* [Link type] [Link to ADR] <!-- example: Refined by [ADR-0005](0005-example.md) -->
* … <!-- numbers of links can vary -->
