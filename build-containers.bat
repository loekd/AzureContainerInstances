docker image build ./azurecontainerinstances.jobgenerator -t loekd/azurecontainerinstances.jobgenerator:latest
docker image build ./azurecontainerinstances.jobgenerator -t loekd/azurecontainerinstances.jobgenerator:1.0

docker push loekd/azurecontainerinstances.jobgenerator:latest
docker push loekd/azurecontainerinstances.jobgenerator:1.0



docker image build ./azurecontainerinstances.jobprocessing -t loekd/azurecontainerinstances.jobprocessing:latest
docker image build ./azurecontainerinstances.jobprocessing -t loekd/azurecontainerinstances.jobprocessing:1.0

docker push loekd/azurecontainerinstances.jobprocessing:latest
docker push loekd/azurecontainerinstances.jobprocessing:1.0



docker image build ./azurecontainerinstances.jobprocessing -t loekd/azurecontainerinstances.logging:latest
docker image build ./azurecontainerinstances.jobprocessing -t loekd/azurecontainerinstances.logging:1.0

docker push loekd/azurecontainerinstances.logging:latest
docker push loekd/azurecontainerinstances.logging:1.0
