# swagger-compiler
A swagger.io compiler giving detailed errors. It won't detect all errors and it is by no means conforming to the swagger standard. It will however complain loudly about the things it finds (or can't find) and give you a "stacktrace" explaining why how it came it that point in the file.

# Example

Source
----------------

    /getInformation:
      get:
        description: Get the information
        parameters:
          - name: id
            type: string
            description: The id
        responses:
          200:
            description: "Success"
            schema:
              $ref: '#/definitions/Information'
          400:
            description: "A general error occurred"
            schema:
              $ref: "#/definitions/Error"
            examples:
              text/plain:
                Description of the error
   
   
Actual error
----------------------

The 'id' parameter is missing the following (required) member "in: query"


Swagger.io editor error:
--------------------------

Refuses to (re)load file. No error given.

Codegen error
---------------------------

    java.lang.NullPointerException
        at io.swagger.codegen.DefaultCodegen.fromParameter(DefaultCodegen.java:923)
        at io.swagger.codegen.DefaultCodegen.fromOperation(DefaultCodegen.java:816)
        at io.swagger.codegen.DefaultGenerator.processOperation(DefaultGenerator.java:352)
        at io.swagger.codegen.DefaultGenerator.processPaths(DefaultGenerator.java:324)
        at io.swagger.codegen.DefaultGenerator.generate(DefaultGenerator.java:153)
        at io.swagger.codegen.cmd.Generate.run(Generate.java:124)
        at io.swagger.codegen.SwaggerCodegen.main(SwaggerCodegen.java:35)


SwaggerCompiler error
------------------------------

    C:\folder\api.yml(144): Found 0 nodes named in, expected 1
    C:\folder\api.yml(144): ...while reading parameter id
    C:\folder\api.yml(141): ...while reading get
    C:\folder\api.yml(18): ...while reading path /getInformation
    C:\folder\api.yml: ...while reading C:\folder\api.yml